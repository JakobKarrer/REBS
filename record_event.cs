namespace HelloWorld
{
    class record_event {
        public string id;
        public string Event;
        public string Title;
        public string role;
        public string  date;
        public string event_name;
        public string event_type;
    public record_event(string id1, string Event1, string Title1, string role1, string date1, string event_name1, string event_type1) {
            id=id1;
            Event = Event1;
            Title = Title1;
            role = role1;
            date = date1;
            event_name = event_name1;
            event_type = event_type1;
    }
  }
}