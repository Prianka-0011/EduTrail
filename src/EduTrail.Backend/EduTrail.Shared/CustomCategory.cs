namespace EduTrail.Shared
{
    public static class CustomCategory
    {
        public static class TermType
        {
            public static Guid Spring { get; set; } = Guid.Parse("7E8F9F7E-75B3-4866-94A3-464F8711C544");
            public static Guid Fall { get; set; } = Guid.Parse("F262DE21-7519-4468-B63A-653DAFC6B8F9");
            public static Guid Winter { get; set; } = Guid.Parse("855021E3-8D31-47B2-B787-65E1DDBB4FE0");
        }

        public static class RoleType
        {
            public static Guid TA { get; set; } = Guid.Parse("5A1E4C7D-9B82-4F36-A3C1-6D9E2F8B0A55");
            public static Guid Instructor { get; set; } = Guid.Parse("8F3B2A91-6E5C-4C7B-9E91-1A2D4F8C3B10");
            public static Guid Student { get; set; } = Guid.Parse("2C9D7F41-8A3E-4F2B-B6A5-9E1C3D4A7F82");
        }

        public static class Months
        {
            public static readonly int January = 1;
            public static readonly int February = 2;
            public static readonly int March = 3;
            public static readonly int April = 4;
            public static readonly int May = 5;
            public static readonly int June = 6;
            public static readonly int July = 7;
            public static readonly int August = 8;
            public static readonly int September = 9;
            public static readonly int October = 10;
            public static readonly int November = 11;
            public static readonly int December = 12;

        }

    }
}