using TuchinC;
using TuchinC.Exceptions;

try
{
    if (args.Length > 1)
    {
        Console.WriteLine("Using tuchin [script]");
    }
    else if (args.Length == 1)
    {
        Tuchin.RunFile(args[0]);
    }
    else
    {
        Tuchin.RunPromt();
    }
}
catch (ParseError pex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Parser error: \r\n{pex.Message}\r\n\r\n{pex}");
}
catch (RuntimeError rex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Runtime error: {rex.Message} with '{rex.Token}'\r\n\r\n{rex}");
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Inner exception: \r\n{ex.Message}\r\n\r\n{ex}\r\n");
}
finally
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.ReadKey();
}

