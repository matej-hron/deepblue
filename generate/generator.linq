<Query Kind="Program" />

static object lck = new object();

string root = @"c:\src\deepblue\generate\";

Dictionary<string, string> pageMap = new Dictionary<string, string> {
	["index.html"] = "content_index.html",
	["clenstvi.html"] = "content_clenstvi.html",
	["lekar.html"] = "content_lekar.html",
	["lekar.html"] = "content_lekar.html",
	["kontakty.html"] = "content_kontakty.html",
	["kurzp1.html"] = "content_p1.html",
	["kurzp2.html"] = "content_p2.html",
	["kurzp3.html"] = "content_p3.html",
	["oxy.html"] = "content_oxy.html",

};

void Main()
{
	GeneratePages();
}

private void WatchFolder()
{

	using var watcher = new FileSystemWatcher(@"c:\src\deepblue\generate\sources");

	watcher.NotifyFilter = NotifyFilters.LastWrite;


	watcher.Changed += OnChanged;

	watcher.Filter = "*.html";
	watcher.IncludeSubdirectories = false;
	watcher.EnableRaisingEvents = true;

	Console.WriteLine("Press enter to exit.");
	Console.ReadLine();
}

private void OnChanged(object sender, FileSystemEventArgs e)
{
	if (e.ChangeType != WatcherChangeTypes.Changed)
	{
		return;
	}
	GeneratePages();
}


void GeneratePages()
{
	lock (lck)
	{
		foreach (var pageKeyValue in pageMap)
		{
			CreatePage(pageKeyValue.Key, pageKeyValue.Value);
		}
		Publish();
		Console.WriteLine($"{DateTime.Now} Web recreated");
	}
}

private void CreatePage(string page, string contentFile)
{
	var header = File.ReadAllText(P("./sources/header.html"));
	var content = File.ReadAllText(P($"./sources/{contentFile}"));
	var footer = File.ReadAllText(P("./sources/footer.html"));

	var html = string.Concat(header, content, footer);
	File.WriteAllText(P($"./result/{page}"), html);
	//Publish();
}

private string P(string path) => Path.Combine(root, path);

public static void Publish()
{
	CopyFolder(@"c:\src\deepblue\generate\result\", @"c:\src\deepblue\");
}

public static void CopyFolder(string sourceFolder, string destFolder)
{

	// Copy all files from source folder to destination
	foreach (string file in Directory.GetFiles(sourceFolder, "*.html"))
	{
		string destFile = Path.Combine(destFolder, Path.GetFileName(file));
		File.Copy(file, destFile, true);  // 'true' to overwrite files if they already exist
		Console.WriteLine($"Coppying {file}");
	}

}

