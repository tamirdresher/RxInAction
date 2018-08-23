namespace Joins
{
    class DoorOpened
    {
        public DoorOpened(string name, Gender gender, OpenDirection direction)
        {
            this.Name = name;
            this.Gender = gender;
            this.Direction = direction;
        }

        public string Name { get; set; }
        public OpenDirection Direction { get; set; }
        public Gender Gender { get; set; }
    }
}
