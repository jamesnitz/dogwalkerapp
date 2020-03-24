using DogWalkerApp.Data;
using DogWalkerApp.Models;
using System;

namespace DogWalkerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Walkers!");

            WalkerRepository walkerRepo = new WalkerRepository();

            OwnerRepository ownerRepo = new OwnerRepository();

            //Inserting an owner
            //Owner newOwner = new Owner() { Name = "my dad", Address = "dads house", NeighborhoodId = 2, Phone = "281-330-8004" };
            //ownerRepo.addOwner(newOwner);

           var allWalkers = walkerRepo.getAllWalkers();
           var allOwners = ownerRepo.getAllOwners();
           var walkersInMemphis = walkerRepo.getWalkersByNeighborhoodId(3);


            foreach (var walker in allWalkers)   
            {
                Console.WriteLine($"{walker.Name} walks around {walker.Neighborhood.Name}");
            }
            Console.WriteLine("");
            Console.WriteLine("");
            foreach (var walker in walkersInMemphis)
            {
                Console.WriteLine($"MEMPHIS WALKER: {walker.Name} walks around {walker.Neighborhood.Name}");
            }
            Console.WriteLine("");
            Console.WriteLine("");
            foreach (var owner in allOwners)
            {
                Console.WriteLine($" Owner: {owner.Name} lives in {owner.Neighborhood.Name}");
            }
            Console.WriteLine("");

            Walker kev = new Walker() { Id = 1, Name = "Kev", NeighborhoodId = 1 };

            walkerRepo.updateWalker(1, kev);

        }
    }
}
