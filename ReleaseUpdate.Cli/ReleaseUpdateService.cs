
using System.IO.Compression;

namespace ReleaseUpdate.Cli;

public static class ReleaseUpdateService
{
    private const string ArgsErrorMessage = "Informe caminhos válidos para as pastas da Release Anterior e arquivo de download da Nova Release! \n";

    public static void Run(string[] args)
    {
        var continuesCheck = true;
        Console.ForegroundColor = ConsoleColor.Blue;
        do
        {
            string previousReleaseFolder = string.Empty;
            string newReleaseFolderWithFileName = string.Empty;    

            if (args.Length == 0 || args.Length == 1)
            {
                Console.WriteLine(ArgsErrorMessage);

                if (string.IsNullOrEmpty(previousReleaseFolder))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Informe o caminho da pasta da Release Anterior: \n");
                    previousReleaseFolder = Console.ReadLine()!.Trim();
                    Console.WriteLine("\n");
                }

                if (!Directory.Exists(previousReleaseFolder))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Pasta da Release Anterior não encontrada! \n");
                    Console.WriteLine("Informe o caminho da pasta da Release Anterior: \n");
                    previousReleaseFolder = Console.ReadLine()!.Trim();
                    Console.WriteLine("\n");
                }

                if (string.IsNullOrEmpty(newReleaseFolderWithFileName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Informe o caminho da pasta e o arquivo de download da Nova Release: \n");
                    newReleaseFolderWithFileName = Console.ReadLine()!.Trim();
                    Console.WriteLine("\n");
                }

                if (!Directory.Exists(Path.GetDirectoryName(newReleaseFolderWithFileName)) || !File.Exists(newReleaseFolderWithFileName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Arquivo da Nova Release não encontrado! \n");
                    Console.WriteLine("Informe o caminho da pasta e arquivo de download da Nova Release: \n");
                    previousReleaseFolder = Console.ReadLine()!.Trim();
                    Console.WriteLine("\n");
                }
            }

            string newReleaseFolderBasePath = Path.GetDirectoryName(newReleaseFolderWithFileName) ?? "";

            if (Directory.Exists(previousReleaseFolder) && 
                Directory.Exists(newReleaseFolderBasePath) && 
                !previousReleaseFolder.Equals("c:", StringComparison.OrdinalIgnoreCase) && 
                !previousReleaseFolder.Equals("c:\\", StringComparison.OrdinalIgnoreCase) && 
                !newReleaseFolderBasePath.Equals("c:", StringComparison.OrdinalIgnoreCase) && 
                !newReleaseFolderBasePath.Equals("c:\\", StringComparison.OrdinalIgnoreCase))
            {

                try
                {
                    string currentFileName = string.Empty;

                    string previousReleaseFolderNewName = previousReleaseFolder + "_Old_" + DateTime.Now.ToString("ddMMyyyy_HHmmss");
                    Directory.Move(previousReleaseFolder, previousReleaseFolderNewName);

                    ZipFile.ExtractToDirectory(newReleaseFolderWithFileName, Path.GetDirectoryName(Path.GetDirectoryName(previousReleaseFolder + Path.DirectorySeparatorChar)) ?? throw new ArgumentNullException());

                    var files = Directory.GetFiles(previousReleaseFolderNewName, "*.*", SearchOption.TopDirectoryOnly)
                                .Where(s => s.EndsWith(".pem") || s.EndsWith(".config"));

                    foreach (var file  in files)
                    {
                        currentFileName = Path.GetFileName(file);
                        File.Copy(file, previousReleaseFolder + Path.DirectorySeparatorChar + currentFileName, true);
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("================================================");
                    Console.WriteLine("Código da aplicação atualizado com sucesso!!! \n");
                    Console.WriteLine("================================================");
                    continuesCheck = false;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("================================================");
                    Console.WriteLine("Ocorreu um erro! " + ex.Message + " \n");
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("================================================");
                }

            }
 
        } while (continuesCheck);

    }
}