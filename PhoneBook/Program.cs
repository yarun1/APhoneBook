using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;

namespace PhoneBook
{
    class Program
    {
        string contactFileName = "contacts.json";
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Contact>));

        static void Main(string[] args)
        {
            string command;
            Program program = new Program();

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

                        Console.Write("Input a contact name: ");
                        contactName = Console.ReadLine();
                        
                        Console.Write("Input a contact phone number: ");
                        contactPhoneNumber = Console.ReadLine();
                        Console.WriteLine();

                        program.CreateContact(new Contact(contactName, contactPhoneNumber));

                        break;
                    }
                    case "e":
                    {
                        string contactData, newContactName, newContactPhoneNumber;

                        Console.Write("Input any contact data to find a contact: ");
                        contactData = Console.ReadLine();

                        Console.Write("Input a new contact name: ");
                        newContactName = Console.ReadLine();

                        Console.Write("Input a new contact phone number: ");
                        newContactPhoneNumber = Console.ReadLine();
                        Console.WriteLine();

                        program.EditContact(contactData, new Contact(newContactName, newContactPhoneNumber));

                        break;
                    }
                    case "r":
                    {
                        string contactData;

                        Console.Write("Input any contact data to find a contact: ");
                        contactData = Console.ReadLine();
                        Console.WriteLine();

                        program.RemoveContact(contactData);

                        break;
                    }
                    case "v":
                    {
                        program.ViewContact();

                        break;
                    }
                    case "q":
                    {
                        return;
                    }
                }
            }
        }

        void SerializeContact(Contact contacToSerialize)
        {
            if (contacToSerialize != null)
            {
                List<Contact> curentContacts = DeserialzeContacts();

                if (curentContacts == null)
                {
                    curentContacts = new List<Contact>();
                }

                curentContacts.Add(contacToSerialize);

                using (FileStream fileStream = new FileStream(contactFileName, FileMode.OpenOrCreate))
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

        void SerializeContacts(List<Contact> contactsToSerialize)
        {
            using (FileStream fileStream = new FileStream(contactFileName, FileMode.Create))
            {
                serializer.WriteObject(fileStream, contactsToSerialize);
                Console.WriteLine("'contactsToSerialize' was successfully serialized");
            }
        }

        List<Contact> DeserialzeContacts()
        {
            FileInfo contactFileInfo = new FileInfo(contactFileName);

            if (contactFileInfo.Exists && contactFileInfo.Length != 0)
            {
                List<Contact> curentContacts;

                using (FileStream fileStream = new FileStream(contactFileName, FileMode.OpenOrCreate))
                {
                    curentContacts = (List<Contact>)serializer.ReadObject(fileStream);
                }

                Console.WriteLine("Contacts were successfully deserialized from '{0}'", contactFileName);

                return curentContacts;
            }
            else
            {
                Console.WriteLine("'{0}' does not exist or it is empty", contactFileName);

                return null;
            }
        }

        void CreateContact(Contact contacToCreate)
        {
            if (contacToCreate != null)
            {
                if (DoesStringContainOnlyDigits(contacToCreate.contactPhoneNumber))
                {
                    if (!string.IsNullOrWhiteSpace(contacToCreate.contactName) && !string.IsNullOrWhiteSpace(contacToCreate.contactPhoneNumber))
                    {
                        SerializeContact(contacToCreate);
                        Console.WriteLine("'contacToCreate' was successfully created");
                    }
                    else
                    {
                        Console.WriteLine("'contactName' or 'contactPhoneNumber' equals null or white space");
                    }
                }
                else
                {
                    Console.WriteLine("'contactPhoneNumber' does not contain only digits");
                }
            }
            else
            {
                Console.WriteLine("'contacToCreate' equals null");
            }
        }

        void EditContact(string contactData, Contact contactNewVersion)
        {
            if (contactNewVersion != null && !string.IsNullOrWhiteSpace(contactData))
            {
                if (!string.IsNullOrWhiteSpace(contactNewVersion.contactName) && !string.IsNullOrWhiteSpace(contactNewVersion.contactPhoneNumber))
                {
                    if (DoesStringContainOnlyDigits(contactNewVersion.contactPhoneNumber))
                    {
                        List<Contact> currentContacts = DeserialzeContacts();

                        if (currentContacts != null)
                        {
                            int editedContactCounter = 0;

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
                                SerializeContacts(currentContacts);
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
                        Console.WriteLine("'contactPhoneNumber' does not contain only digits");
                    }
                    
                }
                else
                {
                    Console.WriteLine("'contactName' or 'contactPhoneNumber' equals null or white space");
                }
            }
            else
            {
                Console.WriteLine("'contactNewVersion' equals null or 'contactData' equlas null or white space");
            }
        }

        void RemoveContact(string contactData)
        {
            if (!string.IsNullOrWhiteSpace(contactData))
            {
                List<Contact> curentContacts = DeserialzeContacts();

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

                    SerializeContacts(editedContacts);
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

        void ViewContact()
        {
            List<Contact> curentContacts = DeserialzeContacts();

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

        bool DoesStringContainOnlyDigits(string stringToCheck)
        {
            foreach (char charToCheck in stringToCheck)
            {
                if (charToCheck < 48 || charToCheck > 57)
                {
                    return false;
                }
            }

            return true;
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
