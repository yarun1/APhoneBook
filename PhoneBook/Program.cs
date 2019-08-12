using System;
using System.IO;

namespace PhoneBook
{
    class Program
    {
        string contactNamesFile = "ContactNames.txt", contactPhoneNumbersFile = "ContactPhoneNumbers.txt";

        static void Main(string[] args)
        {
            string phoneBookCommand;
            Program program = new Program();

            const string CONTACT_CREATING = "c";
            const string CONTACT_EDITING = "e";
            const string CONTACT_REMOVING = "r";
            const string CONTACT_VIEWING = "v";
            const string PROGRAM_EXITING = "q";

            Console.WriteLine("WELCOME TO YOUR PHONE BOOK");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n");
                Console.WriteLine("Press 'c' to create a new contact");
                Console.WriteLine("Press 'e' to edit an existing contact");
                Console.WriteLine("Press 'r' to remove a contact");
                Console.WriteLine("Press 'v' to view all your contacts");
                Console.WriteLine("Press 'q' to quit the program \n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Input a command: ");

                phoneBookCommand = Console.ReadLine();
                Console.WriteLine();

                switch (phoneBookCommand)
                {
                    case CONTACT_CREATING:
                    {
                        string contactNameToCreate, contactPhoneNumberToCreate;

                        Console.Write("Input a contact name: ");
                        contactNameToCreate = Console.ReadLine();

                        Console.Write("Input a contact phone number: ");
                        contactPhoneNumberToCreate = Console.ReadLine();
                        Console.WriteLine();

                        program.CreateContact(contactNameToCreate, contactPhoneNumberToCreate);
                        break;
                    }
                    case CONTACT_EDITING:
                    {
                        string contactData, newContactName, newContactPhoneNumber;

                        Console.Write("Input any contact data to find a contact: ");
                        contactData = Console.ReadLine();

                        Console.Write("Input a new contact name: ");
                        newContactName = Console.ReadLine();

                        Console.Write("Input a new contact phone number: ");
                        newContactPhoneNumber = Console.ReadLine();
                        Console.WriteLine();

                        program.EditContact(contactData, newContactName, newContactPhoneNumber);
                        break;
                    }
                    case CONTACT_REMOVING:
                    {
                        string contactData;

                        Console.Write("Input any contact data to find a contact: ");
                        contactData = Console.ReadLine();
                        Console.WriteLine();

                        program.RemoveContact(contactData);
                        break;
                    }
                    case CONTACT_VIEWING:
                    {
                        program.ViewContactList();
                        break;
                    }
                    case PROGRAM_EXITING:
                    {
                        return;
                    }
                }
            }
        }

        void WriteContact(bool appendContact, string contactName, string contactPhoneNumber)
        {
            using (StreamWriter contactNameStreamWriter = new StreamWriter(contactNamesFile, appendContact))
            {
                contactNameStreamWriter.Write(contactName + ' ');
            }

            using (StreamWriter contactPhoneNumberStreamWriter = new StreamWriter(contactPhoneNumbersFile, appendContact))
            {
                contactPhoneNumberStreamWriter.Write(contactPhoneNumber + ' ');
            }
        }

        string[] ReadContactNames()
        {
            using (StreamReader contactNameStreamReader = new StreamReader(new FileStream(contactNamesFile, FileMode.OpenOrCreate)))
            {
                string currentContactNames = contactNameStreamReader.ReadToEnd().Trim();
                string[] contactNames = currentContactNames.Split();

                return contactNames;
            }
        }

        string[] ReadContactPhoneNumbers()
        {
            using (StreamReader contactPhoneNumberStreamReader = new StreamReader(new FileStream(contactPhoneNumbersFile, FileMode.OpenOrCreate)))
            {
                string currentContactPhoneNumbers = contactPhoneNumberStreamReader.ReadToEnd().Trim();
                string[] contactNames = currentContactPhoneNumbers.Split();

                return contactNames;
            }
        }

        void CreateContact(string contactNameToCreate, string contactPhoneNumberToCreate)
        {
            if (DoesContactNameExist(contactNameToCreate) && DoesContactPhoneNumberExist(contactPhoneNumberToCreate) &&
                DoesStringContainOnlyDigits(contactPhoneNumberToCreate))
            {
                WriteContact(true, contactNameToCreate, contactPhoneNumberToCreate);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The contact was successfully created");
            }
        }

        void EditContact(string contactData, string newContactName, string newContactPhoneNumber)
        {
            string[] currentContactNames = ReadContactNames(), currentContactPhoneNumbers = ReadContactPhoneNumbers();

            if (DoesContactDataExist(contactData) && DoesContactNameExist(newContactName) && DoesContactPhoneNumberExist(newContactPhoneNumber) &&
                DoesStringContainOnlyDigits(newContactPhoneNumber))
            {
                bool isItFirstIteration = true, isContactFound = false;

                for (int i = 0; i < currentContactNames.Length; i++)
                {
                    if (currentContactNames[i] == contactData || currentContactPhoneNumbers[i] == contactData)
                    {
                        currentContactNames[i] = newContactName;
                        currentContactPhoneNumbers[i] = newContactPhoneNumber;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nA contact was successfully edited. A new version of the contact: ");
                        Console.WriteLine(currentContactNames[i]);
                        Console.WriteLine(currentContactPhoneNumbers[i]);

                        isContactFound = true;
                    }

                    if (isItFirstIteration)
                    {
                        WriteContact(false, currentContactNames[i], currentContactPhoneNumbers[i]);
                        isItFirstIteration = false;

                        continue;
                    }

                    WriteContact(true, currentContactNames[i], currentContactPhoneNumbers[i]);
                }

                if (!isContactFound)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No contacts have been found with this contact data: " + contactData);
                }
            }
        }

        void RemoveContact(string contactData)
        {
            string[] currentContactNames = ReadContactNames(), currentContactPhoneNumbers = ReadContactPhoneNumbers();

            if (DoesContactDataExist(contactData) && AreCurrentContactsCorrect(currentContactNames, currentContactPhoneNumbers))
            {
                bool isContactFound = false;

                for (int i = 0; i < currentContactNames.Length; i++)
                {
                    if (currentContactNames[i] != contactData && currentContactPhoneNumbers[i] != contactData)
                    {
                        WriteContact(false, currentContactNames[i], currentContactPhoneNumbers[i]);
                    }
                    else
                    {
                        isContactFound = true;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nA contact was successfully removed: ");
                        Console.WriteLine(currentContactNames[i]);
                        Console.WriteLine(currentContactPhoneNumbers[i]);
                    }
                }

                if (!isContactFound)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No contacts have been found with this contact data: " + contactData);
                }
            }
        }

        void ViewContactList()
        {
            string[] currentContactNames = ReadContactNames(), currentContactPhoneNumbers = ReadContactPhoneNumbers();

            if (AreCurrentContactsCorrect(currentContactNames, currentContactPhoneNumbers))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nCONTACT LIST");

                for (int i = 0; i < currentContactNames.Length; i++)
                {
                    Console.WriteLine("\n" + currentContactNames[i]);
                    Console.WriteLine(currentContactPhoneNumbers[i]);
                }
            }
        }

        bool DoesStringContainOnlyDigits(string stringToCheck)
        {
            const int ASCII_DEC_CODE_ZERO_CHAR = 48;
            const int ASCII_DEC_CODE_NINE_CHAR = 57;

            foreach (char charToCheck in stringToCheck)
            {
                if (charToCheck < ASCII_DEC_CODE_ZERO_CHAR || charToCheck > ASCII_DEC_CODE_NINE_CHAR)
                {
                    return false;
                }
            }

            return true;
        }

        bool DoesContactNameExist(string contactName)
        {
            if (string.IsNullOrWhiteSpace(contactName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There is no contact name");

                return false;
            }

            return true;
        }

        bool DoesContactPhoneNumberExist(string contactPhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(contactPhoneNumber))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There is no contact phone number");

                return false;
            }

            return true;
        }

        bool DoesContactDataExist(string contactData)
        {
            if (string.IsNullOrWhiteSpace(contactData))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There is no contact data to find a contact");

                return false;
            }

            return true;
        }

        bool AreCurrentContactsCorrect(string[] currentContactNames, string[] currentContactPhoneNumbers)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            if (string.IsNullOrEmpty(currentContactNames[0]) || string.IsNullOrEmpty(currentContactPhoneNumbers[0]))
            {
                Console.WriteLine("There is no contacts in the phone book");
                return false;
            }

            if (currentContactNames.Length != currentContactPhoneNumbers.Length)
            {
                Console.WriteLine("Contact names and phone numbers do not correspondent each other");
                return false;
            }

            return true;
        }
    }
}