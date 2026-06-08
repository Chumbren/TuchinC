using Heart.Commands;
using TuchinC;
using TuchinC.Exceptions;

ICommand[] commands = [
    new RunCommand()
    ];

try
{
    if (args.Length == 1)
        throw new ArgumentOutOfRangeException(nameof(args), "Heart - командный менеджер для языка компилятора TuchinC и вертуальной машины TVM");


    string path = args[0];
    foreach (var command in commands)
    {
        string arg = args[1];
        if (command.CommandName == arg)
        {
            List<string> modifiers = [];
            if (args.Length > 2)
            {
                for (int i = 2; i < args.Length; i++)
                {
                    if (args[i].Contains('-'))
                        modifiers.Add(args[i].Replace("-",""));
                }
            }

            command.Execute(path, [.. modifiers]);
        }
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
    Console.WriteLine("Завершенно успешно!");
}

