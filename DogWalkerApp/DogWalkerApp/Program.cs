using DogWalkerApp.Data;
using System;

namespace DogWalkerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Walkers!");

            WalkerRepository walkerRepo = new WalkerRepository();

           var allWalkers = walkerRepo.getAllWalkers();

            foreach(var walker in allWalkers)
            {
                Console.WriteLine($"{walker.Name} walks around {walker.Neighborhood.Name}");
            }
        }
    }
}
