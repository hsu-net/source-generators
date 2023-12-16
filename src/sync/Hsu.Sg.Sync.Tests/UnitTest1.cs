using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace HsuSgSyncTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        // if (node is InterfaceDeclarationSyntax)
        // {
        //     // ReSharper disable once StringLiteralTypo
        //     var exp = SyntaxFactory.ParseExpression("!(NETFRAMEWORK || NETSTANDARD) || NETCOREAPP3_0 || NETCOREAPP3_1");
        //     var @if = SyntaxFactory.IfDirectiveTrivia(exp, true, true, true);
        //     var @endif = SyntaxFactory.EndIfDirectiveTrivia(true);
        //
        //     var triviaIf = SyntaxFactory.Trivia(@if);
        //     var triviaEndIf = SyntaxFactory.Trivia(@endif);
        //
        //     var nodeOldLeadingTrivia = node.GetLeadingTrivia().Add(triviaIf);
        //     var nodeOldTrailingTrivia = node.GetTrailingTrivia().Add(triviaEndIf);
        //
        //     node = node
        //         .WithLeadingTrivia(nodeOldLeadingTrivia)
        //         .WithTrailingTrivia(nodeOldTrailingTrivia);
        // }
    }


    [TestMethod]
    public void MainTest()
    {
        var dir = Path.Combine(Environment.CurrentDirectory,"..","..","..", "Samples");
        var sources = Directory.GetFiles(dir).Select(File.ReadAllText);
        
        GenerateSource(sources,null);
    }
    
    /// <summary>
    /// https://notanaverageman.github.io/2020/12/07/cs-source-generators-cheatsheet.html
    /// </summary>
    /// <param name="sources"></param>
    /// <param name="additionalTextPaths"></param>
    private static void GenerateSource(IEnumerable<string> sources, IEnumerable<string> additionalTextPaths)
    {
        List<MetadataReference> references = new();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            if (!assembly.IsDynamic)
            {
                references.Add(MetadataReference.CreateFromFile(assembly.Location));
            }
        }

        List<SyntaxTree> syntaxTrees = new();
        List<AdditionalText> additionalTexts = new();

        foreach (var source in sources)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);
            syntaxTrees.Add(syntaxTree);
        }

        foreach (var additionalTextPath in additionalTextPaths)
        {
            AdditionalText additionalText = new CustomAdditionalText(additionalTextPath);
            additionalTexts.Add(additionalText);
        }

        var compilation = CSharpCompilation.Create(
            "original",
            syntaxTrees,
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver
            .Create(Array.Empty<ISourceGenerator>())
            //.Create(new Hsu.Sg.Sync.Generator())
            .AddAdditionalTexts(ImmutableArray.CreateRange(additionalTexts));
        
        driver.RunGeneratorsAndUpdateCompilation(
            compilation,
            out var outputCompilation,
            out var diagnostics);

        var hasError = false;

        foreach (var diagnostic in diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error))
        {
            hasError = true;
            Console.WriteLine(diagnostic.GetMessage());
        }

        if(!hasError)
        {
            Console.WriteLine(string.Join("\r\n", outputCompilation.SyntaxTrees));
        }
    }
}

public class CustomAdditionalText : AdditionalText
{
    private readonly string _text;

    public override string Path { get; }

    public CustomAdditionalText(string path)
    {
        Path = path;
        _text = File.ReadAllText(path);
    }

    public override SourceText GetText(CancellationToken cancellationToken = new())
    {
        return SourceText.From(_text);
    }
}
