using System;

namespace BasicAggregateOperators
{
    internal class StudentGrade
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Grade { get; set; }

        public override string ToString()
        {
            return String.Format("Id: {0}, Name: {1}, Grade: {2}", Id, Name, Grade);
        }
    }
}