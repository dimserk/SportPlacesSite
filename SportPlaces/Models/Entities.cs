using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportPlaces.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Login { get; set; }

        [Required]
        [MaxLength(19)]
        public string Phone { get; set; }

        public int? CityId { get; set; }
        public City City { get; set; }

        public List<Record> RecordList { get; set; }
    }

    public class City
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(65)]
        public string CityName { get; set; }

        public List<SportObject> SportObjectList { get; set; }
        public List<User> UserList { get; set; }
    }

    public class SportKind
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string SportKindName { get; set; }
    }

    public class Photo
    {
        public int Id { get; set; }

        [Required]
        public int SportObjectId { get; set; }
        public SportObject SportObject { get; set; }

        [Required]
        public byte[] Image { get; set; }
    }

    public class SportObject
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public string Discription { get; set; }

        [Required]
        public int MaxPeople { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime Beginning { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime Ending { get; set; }

        [Required]
        public double Interval { get; set; }

        [Required]
        public int SportKindId { get; set; }
        public SportKind SportKind { get; set; }

        [Required]
        public int CityId { get; set; }
        public City City { get; set; }

        public List<Photo> PhotoList { get; set; }

        public List<Record> RecordList { get; set; }
    }

    public class Record
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public double Length { get; set; }

        [Required]
        public int SportObjectId { get; set; }
        public SportObject SportObject { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
