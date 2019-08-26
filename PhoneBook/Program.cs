using System;
using System.IO;
using System.Collections.Generic;

namespace PhoneBook
{
    class Program
    {
        string contactsFile = "Contacts.txt";
        
        static void Main(string[] args)
        {
            Program program = new Program();

            string phoneBookCommand;
            string contactName;
            string contactPhoneNumber;
            string contactData;

            const string CONTACT_GETTING_COMMAND = "g";
            const string CONTACT_LIST_GETTING_COMMAND = "gl";
            const string CONTACT_CREATION_COMMAND = "c";
            const string CONTACT_UPDATE_COMMAND = "u";
            const string CONTACT_REMOVING_COMMAND = "r";
            const string CONTACT_LIST_REMOVING_COMMAND = "rl";
            const string PROGRAM_EXIT_COMMAND = "e";

            Console.WriteLine("WELCOME TO YOUR PHONE BOOK");

            while (true)
            {
                Console.WriteLine($"\n\nPress '{CONTACT_GETTING_COMMAND}' to get a contact from the contact list");
                Console.WriteLine($"Press '{CONTACT_LIST_GETTING_COMMAND}' to get the contact list");
                Console.WriteLine($"Press '{CONTACT_CREATION_COMMAND}' to create a new contact");
                Console.WriteLine($"Press '{CONTACT_UPDATE_COMMAND}' to update a contact");
                Console.WriteLine($"Press '{CONTACT_REMOVING_COMMAND}' to remove a contact");
                Console.WriteLine($"Press '{CONTACT_LIST_REMOVING_COMMAND}' to remove the contact list");
                Console.WriteLine($"Press '{PROGRAM_EXIT_COMMAND}' to exit the program \n\n");
                Console.Write("Input a command: ");

                phoneBookCommand = Console.ReadLine();

                switch (phoneBookCommand)
                {
                    case CONTACT_GETTING_COMMAND:
                    {
                        break;
                    }
                    case CONTACT_LIST_GETTING_COMMAND:
                    {
                        program.GetContactList();
                        break;
                    }
                    case CONTACT_CREATION_COMMAND:
                    {
                        Console.Write("Input a contact name: ");
                        contactName = Console.ReadLine();

                        Console.Write("Input a contact phone number: ");
                        contactPhoneNumber = Console.ReadLine();
                        Console.WriteLine();

                        program.CreateContact(new Contact(contactName, contactPhoneNumber));
                        break;
                    }
                    case CONTACT_UPDATE_COMMAND:
                    {
                        Console.Write("Input any contact data to find a contact: ");
                        contactData = Console.ReadLine();

                        Console.Write("Input a new contact name: ");
                        contactName = Console.ReadLine();

                        Console.Write("Input a new contact phone number: ");
                        contactPhoneNumber = Console.ReadLine();
                        Console.WriteLine();

                        program.UpdateContact(contactData, new Contact(contactName, contactPhoneNumber));
                        break;
                    }
                    case CONTACT_REMOVING_COMMAND:
                    {
                        Console.Write("Input any contact data to find a contact: ");
                        contactData = Console.ReadLine();
                        Console.WriteLine();

                        program.RemoveContact(contactData);
                        break;
                    }
                    case CONTACT_LIST_REMOVING_COMMAND:
                    {
                        break;
                    }
                    case PROGRAM_EXIT_COMMAND:
                    {
                        return;
                    }
                    default:
                    {
                        Console.WriteLine("\nThe incorrect command");
                        break;
                    }
                }
            }
        }

        void WriteContact(bool appendContact, Contact contact)
        {
            using (StreamWriter contactStreamWriter = new StreamWriter(contactsFile, appendContact))
            {
                contactStreamWriter.Write(contact.name + ' ');
                contactStreamWriter.Write(contact.phoneNumber + "  ");
            }
        }

        Contact[] ReadContactList()
        {
            using (StreamReader contactStreamReader = new StreamReader(new FileStream(contactsFile, FileMode.OpenOrCreate)))
            {
                string[] currentContacts = contactStreamReader.ReadToEnd().Split(new[] { "  " }, StringSplitOptions.RemoveEmptyEntries);
                Contact[] contacts = new Contact[currentContacts.Length];
                
                for (int i = 0; i < contacts.Length; i++)
                {
                    string[] contact = currentContacts[i].Split();
                    contacts[i] = new Contact(contact[0], contact[1]);
                }

                return contacts;
            }
        }

        List<Contact> SearchContacts(string contactData)
        {
            bool isContactFound = false;

            Contact[] currentContacts = ReadContactList();
            List<Contact> searchedContacts = new List<Contact>();

            foreach (Contact currentContact in currentContacts)
            {
                if (currentContact.name == contactData || currentContact.phoneNumber == contactData)
                {
                    searchedContacts.Add(currentContact);
                    isContactFound = true;
                }
            }

            if (!isContactFound)
            {
                Console.WriteLine("\nThe incorrect contact data");
            }

            return searchedContacts;
        }

        void CreateContact(Contact contacToCreate)
        {
            if (DoesContactExist(contacToCreate) && DoesStringContainOnlyDigits(contacToCreate.phoneNumber))
            {
                WriteContact(true, contacToCreate);
                Console.WriteLine("Succeeded");
            }
        }

        void UpdateContact(string contactData, Contact newContact)
        {
            if (DoContactDataExist(contactData) && DoesContactExist(newContact) && DoesStringContainOnlyDigits(newContact.phoneNumber))
            {
                List<Contact> currentContacts = new List<Contact>(ReadContactList());

                if (currentContacts.Count != 0)
                {
                    List<Contact> searchedContacts = SearchContacts(contactData);

                    for (int i = 0; i < searchedContacts.Count; i++)
                    {
                        currentContacts.RemoveAt(searchedContacts.IndexOf(searchedContacts[i]));
                        currentContacts.Add(newContact);

                        Console.WriteLine("\nSucceeded");
                        Console.WriteLine($"{searchedContacts[i].name} --> {newContact.name}");
                        Console.WriteLine($"{searchedContacts[i].phoneNumber} --> {newContact.phoneNumber}");
                    }

                    WriteContact(false, currentContacts[0]);

                    for (int i = 1; i < currentContacts.Count; i++)
                    {
                        WriteContact(true, currentContacts[i]);
                    }
                }
                else
                {
                    Console.WriteLine("There is no contacts in the phone book");
                }
            }
        }

        void RemoveContact(string contactData)
        {
            if (DoContactDataExist(contactData))
            {
                List<Contact> currentContacts = new List<Contact>(ReadContactList());

                if (currentContacts.Count != 0)
                {
                    List<Contact> searchedContacts = SearchContacts(contactData);

                    for (int i = 0; i < searchedContacts.Count; i++)
                    {
                        currentContacts.RemoveAt(searchedContacts.IndexOf(searchedContacts[i]));
                    }

                    WriteContact(false, currentContacts[0]);

                    for (int i = 1; i < currentContacts.Count; i++)
                    {
                        WriteContact(true, currentContacts[i]);
                    }

                    Console.WriteLine("Succeeded");
                }
                else
                {
                    Console.WriteLine("There is no contacts in the phone book");
                }
            }
        }

        void GetContactList()
        {
            Contact[] currentContacts = ReadContactList();

            Console.WriteLine("\n\nCONTACT LIST");

            for (int i = 0; i < currentContacts.Length; i++)
            {
                Console.WriteLine("\n" + currentContacts[i].name);
                Console.WriteLine(currentContacts[i].phoneNumber);
            }
        }

        bool DoesStringContainOnlyDigits(string stringToCheck)
        {
            foreach (char charToCheck in stringToCheck)
            {
                if (charToCheck < '0' || charToCheck > '9')
                {
                    return false;
                }
            }

            return true;
        }

        bool DoesContactExist(Contact contacToCheck)
        {
            if (string.IsNullOrWhiteSpace(contacToCheck.name))
            {
                Console.WriteLine("There is no contact name");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(contacToCheck.phoneNumber))
            {
                Console.WriteLine("There is no contact phone number");
                return false;
            }

            return true;
        }
       
        bool DoContactDataExist(string contactDataToCheck)
        {
            if (string.IsNullOrWhiteSpace(contactDataToCheck))
            {
                Console.WriteLine("There is no contact data to find a contact");
                return false;
            }

            return true;
        }
    }

    class Contact
    {
        public string name;
        public string phoneNumber;

        public Contact(string name, string phoneNumber)
        {
            this.name = name;
            this.phoneNumber = phoneNumber;
        }
    }
}