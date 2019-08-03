using System;
using System.Collections.Generic;
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
                        string contactName, contactPhoneNumber;
                        CreateContactHandler createContactHandler = new CreateContactHandler();

                        Console.Write("Input contact name: ");
                        contactName = Console.ReadLine();
                        
                        Console.Write("Input contact phone number: ");
                        contactPhoneNumber = Console.ReadLine();
                        Console.WriteLine();

                        createContactHandler.CreateContact(new Contact(contactName, contactPhoneNumber));

                        break;
                    }
                    case "e":
                    {
                        string contactData, newContactName, newContactPhoneNumber;
                        EditContactHandler editContactHandler = new EditContactHandler();

                        Console.Write("Input any contact data to find a contact: ");
                        contactData = Console.ReadLine();

                        Console.Write("Input a new contact name: ");
                        newContactName = Console.ReadLine();

                        Console.Write("Input a new contact phone number: ");
                        newContactPhoneNumber = Console.ReadLine();
                        Console.WriteLine();

                        editContactHandler.EditContact(contactData, new Contact(newContactName, newContactPhoneNumber));

                        break;
                    }
                    case "r":
                    {
                        string contactData;
                        RemoveContactHandler removeContactHandler = new RemoveContactHandler();

                        Console.Write("Input any contact data to find a contact: ");
                        contactData = Console.ReadLine();

                        removeContactHandler.RemoveContact(contactData);

                        break;
                    }
                    case "v":
                    {
                        ViewContactHandler viewContactHandler = new ViewContactHandler();
                        viewContactHandler.ViewContact();

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
        ContactSerialization contactSerialization = new ContactSerialization();

        internal void CreateContact(Contact contacToCreate)
        {
            if (contacToCreate != null)
            {
                if (!string.IsNullOrWhiteSpace(contacToCreate.contactName) && !string.IsNullOrWhiteSpace(contacToCreate.contactPhoneNumber))
                {
                    contactSerialization.SerializeContact(contacToCreate);
                    Console.WriteLine("A contact was successfully created \n");
                }
                else
                {
                    Console.WriteLine("'contactName' or 'contactPhoneNumber' is null or empty");
                }
            }
            else
            {
                Console.WriteLine("'contacToCreate' references to null \n");
            }
        }
    }

    class EditContactHandler
    {
        ContactSerialization contactSerialization = new ContactSerialization();

        internal void EditContact(string contactData, Contact contactNewVersion)
        {
            if (!string.IsNullOrWhiteSpace(contactData) && contactNewVersion != null)
            {
                List<Contact> curentContacts = contactSerialization.DeserialzeContacts();

                if (curentContacts != null)
                {
                    foreach (char contactPhoneNumberSymbol in contactNewVersion.contactPhoneNumber)
                    {
                        if (contactPhoneNumberSymbol < 0 || contactPhoneNumberSymbol > 9)
                        {
                            Console.WriteLine("A new phone number is incorrect");

                            return;
                        }
                    }

                    foreach (Contact cuurentContact in curentContacts)
                    {
                        if (cuurentContact.contactName == contactData || cuurentContact.contactPhoneNumber == contactData)
                        {
                            cuurentContact.contactName = contactNewVersion.contactName;
                            cuurentContact.contactPhoneNumber = contactNewVersion.contactPhoneNumber;

                            Console.WriteLine("A contact was successfully edited:");
                            Console.WriteLine(contactNewVersion.contactName);
                            Console.WriteLine(contactNewVersion.contactPhoneNumber + "\n");
                        }
                    }

                    contactSerialization.SerializeContacts(curentContacts);
                }
                else
                {
                    Console.WriteLine("'curentContacts' references to null \n");
                }
            }
            else
            {
                Console.WriteLine("No contact data or 'contactNewVersion' references to null \n");
            }
        }
    }

    class RemoveContactHandler
    {
        ContactSerialization contactSerialization = new ContactSerialization();

        internal void RemoveContact(string contactData)
        {
            if (!string.IsNullOrWhiteSpace(contactData))
            {
                List<Contact> curentContacts = contactSerialization.DeserialzeContacts();

                if (curentContacts != null)
                {
                    List<Contact> editedContacts = new List<Contact>(curentContacts);

                    foreach (Contact curentContact in curentContacts)
                    {
                        if (curentContact.contactName == contactData || curentContact.contactPhoneNumber == contactData)
                        {
                            editedContacts.Remove(curentContact);

                            Console.WriteLine("A contact was successfully removed:");
                            Console.WriteLine(curentContact.contactName);
                            Console.WriteLine(curentContact.contactPhoneNumber + "\n");
                        }
                    }

                    contactSerialization.SerializeContacts(editedContacts);
                }
                else
                {
                    Console.WriteLine("'curentContacts' references to null \n");
                }
            }
            else
            {
                Console.WriteLine("No contact data or 'contactNewVersion' references to null \n");
            }
        }
    }

    class ViewContactHandler
    {
        ContactSerialization contactSerialization = new ContactSerialization();

        internal void ViewContact()
        {
            List<Contact> curentContacts = contactSerialization.DeserialzeContacts();

            if (curentContacts != null)
            {
                foreach (Contact curentContact in curentContacts)
                {
                    Console.WriteLine(curentContact.contactName);
                    Console.WriteLine(curentContact.contactPhoneNumber + "\n");
                }
            }
            else
            {
                Console.WriteLine("No contact data or 'contactNewVersion' references to null \n");
            }
        }
    }

    class ContactSerialization
    {
        string contactFileName = "contacts.json";
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Contact>));

        internal void SerializeContact(Contact contacToSerialize)
        {
            if (contacToSerialize != null)
            {
                List<Contact> curentContacts = DeserialzeContacts();
                
                if (curentContacts == null)
                {
                    curentContacts = new List<Contact>();
                }

                curentContacts.Add(contacToSerialize);

                using (FileStream fileStream = new FileStream("contacts.json", FileMode.OpenOrCreate))
                {
                    serializer.WriteObject(fileStream, curentContacts);
                }

                Console.WriteLine("A contact was successfully serialized \n");
            }
            else
            {
                Console.WriteLine("'contacToSerialize' references to null \n");
            }
        }

        internal void SerializeContacts(List<Contact> contactsToSerialize)
        {
            using (FileStream fileStream = new FileStream("contacts.json", FileMode.Create))
            {
                serializer.WriteObject(fileStream, contactsToSerialize);
            }
        }

        internal List<Contact> DeserialzeContacts()
        {
            FileInfo contactFileInfo = new FileInfo(contactFileName);

            if (contactFileInfo.Exists && contactFileInfo.Length != 0)
            {
                List<Contact> curentContacts;

                using (FileStream fileStream = new FileStream(contactFileName, FileMode.OpenOrCreate))
                {
                    curentContacts = (List<Contact>)serializer.ReadObject(fileStream);
                }

                Console.WriteLine("Contacts were successfully deserialized \n");

                return curentContacts;
            }
            else
            {
                Console.WriteLine("File can not be deserialized, because it is empty. Returning null... \n");

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
