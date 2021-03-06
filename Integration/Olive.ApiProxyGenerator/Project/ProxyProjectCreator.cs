﻿using System;

namespace Olive.ApiProxy
{
    class ProxyProjectCreator : ProjectCreator
    {
        public ProxyProjectCreator() : base("Proxy") { }

        protected override string Framework => "netstandard2.0";

        protected override string IconUrl => null;

        protected override string[] References
            => new[] { "Olive", "Olive.Entities", "Olive.Entities.Data", "Olive.ApiClient", "Olive.Microservices" };

        protected override void AddFiles()
        {
            Console.Write("Adding the proxy class...");
            Folder.GetFile($"{Context.ControllerName}.cs").WriteAllText(ProxyClassProgrammer.Generate());
            Console.WriteLine("Done");

            GenerateDtoClasses();
            GenerateDataProviderClasses();
        }

        void GenerateDtoClasses()
        {
            foreach (var type in DtoTypes.All)
            {
                Console.Write("Adding DTO class " + type.Name + "...");
                var dto = new DtoProgrammer(type);
                Folder.GetFile(type.Name + ".cs").WriteAllText(dto.Generate());
                Console.WriteLine("Done");
            }
        }

        void GenerateDataProviderClasses()
        {
            foreach (var type in DtoTypes.All)
            {
                Console.Write("Adding DTO class " + type.Name + "...");
                var dto = new DtoProgrammer(type);
                Folder.GetFile(type.Name + ".cs").WriteAllText(dto.Generate());

                var dataProvider = new DtoDataProviderClassGenerator(type).Generate();
                if (dataProvider.HasValue())
                    Folder.GetFile(type.Name + "DataProvider.cs").WriteAllText(dataProvider);

                Console.WriteLine("Done");
            }
        }
    }
}