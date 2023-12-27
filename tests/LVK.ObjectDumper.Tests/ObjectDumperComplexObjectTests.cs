namespace LVK.ObjectDumper.Tests;

public class ObjectDumperComplexObjectTests : ObjectDumperTestBase
{
    [Test]
    public void Dump()
    {
        object input = new
        {
            What = 42,
            Why = "Meaning of life",
            Who = new
            {
                Name = "Douglas Adams",
                Books = new[]
                {
                    "The Hitchhiker's Guide to the Galaxy", "The Restaurant at the End of the Universe", "Life, the Universe and Everything", "So Long, and Thanks for All the Fish", "Mostly Harmless",
                },
            },
        };

        string output = Dumper.Dump(input);

        var expected = """
                       <0> input = { What = 42, Why = Meaning of life, Who = { Name = Douglas Adams, Books = System... [.<>f__AnonymousType0<System.Int32, System.String, .<>f__AnonymousType1<System.String, System.String[]>>]
                       {
                           properties
                           {
                               What = 42 [System.Int32]
                               <1> Why = "Meaning of life" [System.String]
                               <2> Who = { Name = Douglas Adams, Books = System.String[] } [.<>f__AnonymousType1<System.String, System.String[]>]
                               {
                                   properties
                                   {
                                       <3> Name = "Douglas Adams" [System.String]
                                       <4> Books = Count: 5 [System.String[]]
                                       {
                                           items
                                           {
                                               <5> #0 = "The Hitchhiker's Guide to the Galaxy" [System.String]
                                               <6> #1 = "The Restaurant at the End of the Universe" [System.String]
                                               <7> #2 = "Life, the Universe and Everything" [System.String]
                                               <8> #3 = "So Long, and Thanks for All the Fish" [System.String]
                                               <9> #4 = "Mostly Harmless" [System.String]
                                           } // items
                                       }
                                   } // properties
                               }
                           } // properties
                       }
                       """;

        Assert.That(output, Is.EqualTo(expected));
    }
}