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

            Console.WriteLine("WELCOME TO YOUR PHONE BOOK");

            while (true)
            {
                Console.WriteLine("\nPress 'c' to create a new contact \nPress 'e' to edit an existing contact \nPress 'r' to remove a contact \nPress 'v' to view all your contacts \nPress 'q' to quit the program \n");
                Console.Write("Input a command: ");

                command = Console.ReadLine();
                Console.WriteLine();

                switch (command)
                {
                    case "c":
                    {
                        string contactName, contactPhoneNumber;
                        CreateContactHandler createContactHandler = new CreateContactHandler();

                        Console.Write("Input a contact name: ");
                        contactName = Console.ReadLine();
                        
                        Console.Write("Input a contact phone number: ");
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
                        Console.WriteLine();

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
                foreach (char contactPhoneNumberSymbol in contacToCreate.contactPhoneNumber)
                {
                    if (contactPhoneNumberSymbol < 48 || contactPhoneNumberSymbol > 57)
                    {
                        Console.WriteLine("'contactPhoneNumber' needs to contain only digits");

                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(contacToCreate.contactName) && !string.IsNullOrWhiteSpace(contacToCreate.contactPhoneNumber))
                {
                    contactSerialization.SerializeContact(contacToCreate);
                    Console.WriteLine("'contacToCreate' was successfully created");
                }
                else
                {
                    Console.WriteLine("'contactName' or 'contactPhoneNumber' equals null or white space");
                }
            }
            else
            {
                Console.WriteLine("'contacToCreate' equals null");
            }
        }
    }

    class EditContactHandler
    {
        ContactSerialization contactSerialization = new ContactSerialization();

        internal void EditContact(string contactData, Contact contactNewVersion)
        {
            if (contactNewVersion != null && !string.IsNullOrWhiteSpace(contactData))
            {
                if (!string.IsNullOrWhiteSpace(contactNewVersion.contactName) && !string.IsNullOrWhiteSpace(contactNewVersion.contactPhoneNumber))
                {
                    int editedContactCounter = 0;
                    List<Contact> currentContacts = contactSerialization.DeserialzeContacts();

                    if (currentContacts != null)
                    {
                        foreach (char contactPhoneNumberSymbol in contactNewVersion.contactPhoneNumber)
                        {
                            if (contactPhoneNumberSymbol < 48 || contactPhoneNumberSymbol > 57)
                            {
                                Console.WriteLine("'contactPhoneNumber' needs to contain only digits");

                                return;
                            }
                        }

                        foreach (Contact currentContact in currentContacts)
                        {
                            if (currentContact.contactName == contactData || currentContact.contactPhoneNumber == contactData)
                            {
                                editedContactCounter++;
                                
                                currentContact.contactName = contactNewVersion.contactName;
                                currentContact.contactPhoneNumber = contactNewVersion.contactPhoneNumber;

                                Console.WriteLine("'curentContact' was successfully edited:");
                                Console.WriteLine(contactNewVersion.contactName);
                                Console.WriteLine(contactNewVersion.contactPhoneNumber + "\n");
                            }
                        }

                        if (editedContactCounter > 0)
                        {
                            contactSerialization.SerializeContacts(currentContacts);
                            Console.WriteLine("'curentContacts' was successfully serialized");
                        }
                        else
                        {
                            Console.WriteLine("No contacts were not found with this 'contactData'");
                        }
                    }
                    else
                    {
                        Console.WriteLine("'curentContacts' equals null");
                    }
                }
                else
                {
                    Console.WriteLine("'contactName' or 'contactPhoneNumber' equals null or white space");
                }
            }
            else
            {
                Console.WriteLine("'contactNewVersion' equals null");
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

                            Console.WriteLine("'curentContact' was successfully removed:");
                            Console.WriteLine(curentContact.contactName);
                            Console.WriteLine(curentContact.contactPhoneNumber + "\n");
                        }
                    }

                    contactSerialization.SerializeContacts(editedContacts);
                }
                else
                {
                    Console.WriteLine("'curentContacts' equals null");
                }
            }
            else
            {
                Console.WriteLine("'contactData' equals null or white space");
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
                Console.WriteLine("\nCONTACT LIST \n");

                foreach (Contact curentContact in curentContacts)
                {
                    Console.WriteLine(curentContact.contactName);
                    Console.WriteLine(curentContact.contactPhoneNumber + "\n");
                }
            }
            else
            {
                Console.WriteLine("'contactNewVersion' equals null");
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

                Console.WriteLine("'contacToSerialize' was successfully serialized");
            }
            else
            {
                Console.WriteLine("'contacToSerialize' equals null");
            }
        }

        internal void SerializeContacts(List<Contact> contactsToSerialize)
        {
            using (FileStream fileStream = new FileStream("contacts.json", FileMode.Create))
            {
                serializer.WriteObject(fileStream, contactsToSerialize);
                Console.WriteLine("'contactsToSerialize' was successfully serialized");
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

                Console.WriteLine("Contacts were successfully deserialized from 'contacts.json'");

                return curentContacts;
            }
            else
            {
                Console.WriteLine("'contacts.json' does not exist or it is empty");

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
