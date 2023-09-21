<Query Kind="Program" />

string root = @"c:\src\deepblue\generate\";

Dictionary<string, string> pageMap = new Dictionary<string, string> {
	["index.html"] = "content_index.html",
	["bazen.html"] = "content_p1.html",
};

void Main()
{
	foreach(var pageKeyValue in pageMap)
	{
		CreatePage(pageKeyValue.Key, pageKeyValue.Value);
	}
}

private void CreatePage(string page, string contentFile)
{
	var header = File.ReadAllText(P("./sources/header.html"));
	var content = File.ReadAllText(P($"./sources/{contentFile}"));
	var footer = File.ReadAllText(P("./sources/footer.html"));

	var html = string.Concat(header, content, footer);
	File.WriteAllText(P($"./result/{page}"), html);
}

private string P(string path) => Path.Combine(root, path);

