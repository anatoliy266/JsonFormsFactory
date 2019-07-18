using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonFormsFactory
{
    public class FormsFactory
    {
        //string json = "{[Label: {name: label1, text: HelloWorld from formsFactory, anchor: [top, left], }, RichTextBox: {width: 100, height: 50, anchors: [top, label1]}]}";
        string json = "{1: [{Label:  {Name: \"label1\", Text: \"HelloWorld from formsFactory\"}}, {RichTextBox : {Width: 150, Height: 50, Name: \"RichTextBox1\" }}], " +
            "2: {ComboBox: {Name: \"ComboBox1\", Width: 100, Height: 40, Items: [\"Engineer\", \"Worker\", \"Student\", \"None\"]}}}";
        ComboBox box = new ComboBox();
        
        public FormsFactory()
        {
            Debug.WriteLine("here: create FormsFactory");
            SQLConnector db = new SQLConnector("localhost\\SQLEXPRESS", "sa", "123456");
            ComboBox box = new ComboBox();
            box.Items.Add("");
        }

        public Form Generate()
        {

            return GenerateForm(ParseJson());
        }

        private object ParseJson()
        {
            try
            {
                Debug.WriteLine("here: parseJson");
                MessageBox.Show("here", "info");
                var result = JsonConvert.DeserializeObject(json);

                return result;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "error");
                return null;
            }

        }

        private Form GenerateForm(object jObj)
        {
            Form gForm = new Form();
            gForm.Width = 200;
            gForm.Height = 400;

            Button save = new Button();
            save.Width = 50;
            save.Height = 20;
            save.Location = new System.Drawing.Point(0, 0);
            save.Left = 0;
            save.Top = gForm.Height - save.Height;
            //save.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
            save.Text = "Save";

            save.Parent = gForm;
            //gForm.Controls.Add(save);

            Button delete = new Button();
            delete.Width = 50;
            delete.Height = 20;
            delete.Location = new System.Drawing.Point(0, 0);
            //delete.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            delete.Left = gForm.Width - delete.Width;
            delete.Top = gForm.Height - delete.Height;
            delete.AutoSize = true;
            delete.Text = "Delete";

            delete.Parent = gForm;
            //gForm.Controls.Add(delete);
            object control = null;

            int x = 0;
            int y = 0;

            var root = (JObject)jObj.GetType().InvokeMember("Root", BindingFlags.GetProperty, null, jObj, null);


            foreach (var val in root.Properties())
            {
                if (val.Value.Type == JTokenType.Array)
                {
                    var array = (JArray)val.Value;
                    foreach (var element in array.Children<JObject>())
                    {
                        var item = CreateControl(element);
                        var placedItem = SetControlPosition(item, x, y);
                        control = placedItem;
                        var dY = Convert.ChangeType(control.GetType().GetProperty("Height").GetValue(control), typeof(Int32));
                        y += (int)dY;
                    }
                    y = 0;
                }
                else if (val.Value.Type == JTokenType.Object)
                {
                    var item = CreateControl((JObject)val.Value);
                    var placedItem = SetControlPosition(item, x, y);
                    control = placedItem;
                }
                else throw new Exception("invalid json");
                var dX = Convert.ChangeType(control.GetType().GetProperty("Height").GetValue(control), typeof(Int32));
                x += (int)dX;

                dynamic formControl = control;
                gForm.Controls.Add(formControl);

            }
            gForm.Update();
            return gForm;
        }


        private object CreateControl(JObject jItem)
        {
            object control = null;
            foreach (var val in jItem.Properties())
            {
                MessageBox.Show(val.Name + ":::" + val.Value, "INFO");
                Type type = Type.GetType("System.Windows.Forms." + val.Name + ", System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", true, true);
                MessageBox.Show(type.ToString(), "INFO");
                var createInstance = typeof(Activator).GetMethods().FirstOrDefault(mi => mi.Name == "CreateInstance" && mi.GetParameters().Count() == 0);
                var createInstanceGen = createInstance.MakeGenericMethod(type);
                var item = createInstanceGen.Invoke(null, null);

                if (val.Value.Type == JTokenType.Object)
                {
                    control = AddProperties(item, (JObject)val.Value);
                }
                else
                {
                    throw new Exception("invalid json");
                }
            }
            return control;
        }

        private object AddProperties(object target, JObject properties)
        {
            object item = target;
            if (properties != null)
            {

                var positionProperty = item.GetType().GetProperty("Location");
                positionProperty.SetValue(item, new System.Drawing.Point(0, 0));
                foreach (var property in properties.Properties())
                {
                    MessageBox.Show(property.Name + ":::" + property.Value);
                    if (property.Value.Type == JTokenType.Array)
                    {
                        var propertyItem = item.GetType().GetProperty(property.Name);
                        //List<string> items = new List<string>();
                        foreach (var arrayElement in property.Value)
                        {
                            MessageBox.Show(arrayElement.ToString(), "INFO");
                            var itemsMethod = propertyItem.GetMethod.Invoke(item, null);
                            itemsMethod.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, itemsMethod, new object[] { arrayElement });
                            //items.Add(arrayElement.ToString());
                        }
                        //propertyItem.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, propertyItem, new object[] { items });
                        
                    } else if(property.Value.Type == JTokenType.Object)
                    {
                        throw new Exception("invalid json");
                    } else
                    {
                        var propertyItem = item.GetType().GetProperty(property.Name);
                        var propertyType = propertyItem.PropertyType;
                        propertyItem.SetValue(item, Convert.ChangeType(property.Value, propertyType));
                    }
                    
                }
            }
            return item;
        }

        private object SetControlPosition(object target, int x, int y)
        {
            var item = target;
            item.GetType().GetProperty("Left").SetValue(item, x);
            item.GetType().GetProperty("Top").SetValue(item, y);
            return item;
        }
    }
        
}
