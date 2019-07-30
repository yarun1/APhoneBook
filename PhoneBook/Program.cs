using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;

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
        internal void CreateContact(Contact contacToCreate)
        {
            if (contacToCreate != null)
            {

            }
            else
            {
                Console.WriteLine("'contacToCreate' references to null");
            }
        }
    }

    class EditContactHandler
    {
        internal void EditContact(string contactData, Contact contactNewVersion)
        {
            if (!string.IsNullOrWhiteSpace(contactData) && contactNewVersion != null)
            {

            }
            else
            {
                Console.WriteLine("No contact data or 'contactNewVersion' references to null");
            }
        }
    }

    class RemoveContactHandler
    {
        internal void RemoveContact(string contactData, Contact contactNewVersion)
        {
            if (!string.IsNullOrWhiteSpace(contactData) && contactNewVersion != null)
            {

            }
            else
            {
                Console.WriteLine("No contact data or 'contactNewVersion' references to null");
            }
        }
    }

    class ViewContactHandler
    {
        internal void ViewContact()
        {

        }
    }

    class ContactSerialization
    {
        string contactFileName = "contacts.json";
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Contact[]));

        internal void SerializeContact(Contact contacToSerialize)
        {
            if (contacToSerialize != null)
            {
                using (FileStream fileStream = new FileStream("contacts.json", FileMode.OpenOrCreate))
                {
                    serializer.WriteObject(fileStream, contacToSerialize);
                }

                Console.WriteLine("A contact was successfully serialized");
            }
            else
            {
                Console.WriteLine("'contacToSerialize' references to null");
            }
        }

        internal Contact[] DeserialzeContacts()
        {
            if (new FileInfo(contactFileName).Length != 0)
            {
                Contact[] deserialzedContacts;

                using (FileStream fileStream = new FileStream(contactFileName, FileMode.OpenOrCreate))
                {
                    deserialzedContacts = (Contact[])serializer.ReadObject(fileStream);
                }

                Console.WriteLine("A contact was successfully deserialized");

                return deserialzedContacts;
            }
            else
            {
                Console.WriteLine("File can not be deserialized, because it is empty. Returning null...");

                return null;
            }
        }
    }

    [DataContract]
    class Contact
    {
        [DataMember]
        internal string contactName;

        [DataMember]
        internal string contactPhoneNumber;

        internal Contact(string contactName, string contactPhoneNumber)
        {
            this.contactName = contactName;
            this.contactPhoneNumber = contactPhoneNumber;
        }
    }
}
