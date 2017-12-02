namespace Prevea.ConsoleTestModel
{
    #region Using

    using System;
    using Repository.Repository;

    #endregion

    class Program
    {
        static void Main()
        {
            Console.WriteLine("*******************************************");
            Console.WriteLine("*         Process Test Repository         *");
            Console.WriteLine("*******************************************");

            Console.WriteLine("Presiona una tecla para comenzar ...");
            Console.ReadLine();

            if (Process())
            {
                Console.WriteLine(String.Empty);
                Console.WriteLine("*******************************************");
                Console.WriteLine("*          ¡¡¡¡Process OK!!!!!            *");
                Console.WriteLine("*******************************************");
                Console.WriteLine(String.Empty);
            }
            else
            {
                Console.WriteLine(String.Empty);
                Console.WriteLine("*******************************************");
                Console.WriteLine("*          ¡¡¡¡Process FAIL!!!!!          *");
                Console.WriteLine("*******************************************");
                Console.WriteLine(String.Empty);
            }

            Console.WriteLine("Presiona una tecla para salir ...");

            Console.ReadLine();

        }

        private static bool Process()
        {
            try
            {
                var repository = new Repository();
                var users = repository.GetUsers();
                foreach (var user in users)
                {
                    Console.WriteLine("-------");
                    Console.WriteLine("|USERS|");
                    Console.WriteLine("-------");
                    Console.WriteLine("User: {0} {1}", user.FirstName, user.LastName);
                    Console.WriteLine(String.Empty);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
                return false;
            }
            return true;
        }
    }
}
