using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportPlaces.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        [Display(Name = "Имя пользователя")]
        public string Login { get; set; }

        [Required]
        [MaxLength(19)]
        [Display(Name = "Телефонный номер")]
        public string Phone { get; set; }

        public int? CityId { get; set; }
        [Display(Name = "Город")]
        public City City { get; set; }

        public List<Record> RecordList { get; set; }
    }

    public class City
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(65)]
        [Display(Name = "Название")]
        public string CityName { get; set; }

        public List<SportObject> SportObjectList { get; set; }
        public List<User> UserList { get; set; }
    }

    public class SportKind
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        [Display(Name = "Вид спорта")]
        public string SportKindName { get; set; }
    }

    public class Photo
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Спортивный объект")]
        public int SportObjectId { get; set; }

        [Display(Name = "Спортивный объект")]
        public SportObject SportObject { get; set; }

        [Required]
        [Display(Name = "Фотография")]
        public byte[] Image { get; set; }
    }

    public class SportObject
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Discription { get; set; }

        [Required]
        [Display(Name = "Максимальное число занимающихся")]
        public int MaxPeople { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Начало занятий")]
        public DateTime Beginning { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Окончание занятий")]
        public DateTime Ending { get; set; }

        [Required]
        [Display(Name = "Длительнось занятия")]
        public double Interval { get; set; }

        [Required]
        public int SportKindId { get; set; }

        [Display(Name = "Вид спорта")]
        public SportKind SportKind { get; set; }

        [Required]
        public int CityId { get; set; }

        [Display(Name = "Город")]
        public City City { get; set; }

        public List<Photo> PhotoList { get; set; }

        public List<Record> RecordList { get; set; }
    }

    public class Record
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Время записи")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Длительность")]
        public double Length { get; set; }

        [Required]
        public int SportObjectId { get; set; }

        [Display(Name = "Спортивный объект")]
        public SportObject SportObject { get; set; }

        [Required]
        public int UserId { get; set; }

        [Display(Name = "Пользователь")]
        public User User { get; set; }
    }
}
