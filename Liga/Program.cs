using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Liga
{
    class Person
    {
        public string id;
        public string name;
        public string sex;
    }

    class PersonsInfo
    {
        public string name;
        public string sex;
        public int age;
    }
    class Program
    {
        static void Main(string[] args)
        {
            //  Делаем запрос и сохраняем полученные данынне в переменной responseFromServer
            WebRequest request = WebRequest.Create("http://testlodtask20172.azurewebsites.net/task");
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            //  Десериализуем массив в формате JSON с помощью Newtonsoft.Json, сохраняя
            //  данные в список c экземплярами класса Person
            List<Person> persons = JsonConvert.DeserializeObject<List<Person>>(responseFromServer);

            Person MaleMinAge = new Person();
            Person FemaleMinAge = new Person();
            int malesMinAge = 120; 
            int femalesMinAge = 120;
            PersonsInfo onesPersonInfo;

            foreach (Person aPerson in persons)
            {
                //  Делаем запрос по id
                request = WebRequest.Create("http://testlodtask20172.azurewebsites.net/task/" + aPerson.id);
                response = request.GetResponse();
                dataStream = response.GetResponseStream();
                reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();

                onesPersonInfo = JsonConvert.DeserializeObject<PersonsInfo>(responseFromServer);

                //  Поиск
                if (onesPersonInfo.sex == "male")
                {
                    if (onesPersonInfo.age < malesMinAge)
                    {
                        malesMinAge = onesPersonInfo.age;
                        MaleMinAge = aPerson;
                    }
                }
                else 
                {
                    if (onesPersonInfo.age < femalesMinAge)
                    {
                        femalesMinAge = onesPersonInfo.age;
                        FemaleMinAge = aPerson;
                    }
                }
            }

            //  Вывод
            Console.WriteLine("The youngest boy is {0}.", MaleMinAge.name);
            Console.WriteLine("The youngest girl is {0}.", FemaleMinAge.name);

            reader.Close();
            response.Close();

            Console.ReadKey();
        }
    }
}
