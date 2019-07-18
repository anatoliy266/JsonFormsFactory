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
        string json = "{Label:  {Name: \"label1\", Text: \"HelloWorld from formsFactory\"}, RichTextBox : {Width: 150, Height: 50, Name: \"RichTextBox1\" }}";

        public FormsFactory()
        {
            Debug.WriteLine("here: create FormsFactory");
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
            } catch (Exception e)
            {
                MessageBox.Show(e.Message, "error");
                return null;
            }
            
        }

        private Form GenerateForm(object jObj)
        {
            Form gForm = new Form();

            

            var root = (JObject)jObj.GetType().InvokeMember("Root", BindingFlags.GetProperty, null, jObj, null);

            foreach (var val in root.Properties())
            {
                MessageBox.Show(val.Name + ":::" + val.Value, "INFO");
                Type type = Type.GetType("System.Windows.Forms."+val.Name+", System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", true, true);
                MessageBox.Show(type.ToString(), "INFO");
                //var item = Assembly.GetExecutingAssembly().CreateInstance("System.Windows.Forms." + val.Name + ", System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", true);
                //MessageBox.Show(val.Value.Type.ToString(), "INFO"); MessageBox.Show(item.GetType().ToString(), "INFO");
                var createInstance = typeof(Activator).GetMethods().FirstOrDefault(mi => mi.Name == "CreateInstance" && mi.GetParameters().Count() == 0);
                var createInstanceGen = createInstance.MakeGenericMethod(type);
                var item = createInstanceGen.Invoke(null, null);
                MessageBox.Show(val.Value.Type.ToString(), "INFO"); MessageBox.Show(item.GetType().ToString(), "INFO");

                dynamic control = null;
                if (val.Value.Type == JTokenType.Object)
                {
                    var properties = (JObject)val.Value;
                    foreach (var property in properties.Properties())
                    {
                        MessageBox.Show(property.Name + ":::" + property.Value, "INFO");
                        Label lb = new Label();
                        var propertyItem = item.GetType().GetProperty(property.Name);
                        var propertyType = propertyItem.PropertyType;
                        propertyItem.SetValue(item, Convert.ChangeType(property.Value, propertyType));
                        control = item;
                    }
                }
                gForm.Controls.Add(control);
                
            }

            return gForm;
        }
    }
}
