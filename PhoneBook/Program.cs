using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;


namespace PhoneBook
{
    class Program
    {
        static void Main(string[] args)
        {
            string command;

            Console.WriteLine("WELCOME TO YOUR PHONE BOOK \n");

            while (true)
            {
                Console.WriteLine("\nPress 'c' to create a new contact \nPress 'e' to edit an existing contact \nPress 'r' to remove an existing contact \nPress 'v' to view all your contacts \nPress 'q' to quit the program \n");
                Console.Write("Input command: ");

                command = Console.ReadLine();
                Console.WriteLine();

                switch (command)
                {
                    case "c":
                    {


                        break;
                    }
                    case "e":
                    {


                        break;
                    }
                    case "r":
                    {


                        break;
                    }
                    case "v":
                    {


                        break;
                    }
                    case "q":
                    {
                        return;
                    }
                }
            }
        }
    }

    class CreateContactHandler
    {

    }

    class EditContactHandler
    {

    }

    class RemoveContactHandler
    {

    }

    class ViewContactHandler
    {

    }

    class ContactSerialization
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Contact[]));

        internal void SerializeContact(Contact contacToSerialize)
        {

        }

        internal Contact[] DeserialzeContacts()
        {

        }
    }

    [DataContract]
    class Contact
    {
        [DataMember]
        public string ContactName { get; private set; }

        [DataMember]
        public string ContactPhoneNumber { get; private set; }


        internal Contact(string contactName, string contactPhoneNumber)
        {
            ContactName = contactName;
            ContactPhoneNumber = contactPhoneNumber;
        }
    }
}