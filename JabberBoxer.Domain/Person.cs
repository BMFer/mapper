using System;

namespace JabberBoxer.Domain
{
    public sealed class Person
    {
        public string Address { get; set; }
        public DateTime CreateTime { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string Phone { get; set; }

        public static Person Create()
        {
            return new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Jerrame",
                LastName = "Hertz",
                Email = "jerrame.hertz@cvshealth.com",
                Address = "8551 Rainbow Ave. Thornton, CO. 80229",
                CreateTime = DateTime.Now,
                Nickname = "JerrameMapper",
                Phone = "720.209.5526"
            };
        }
    }
}
