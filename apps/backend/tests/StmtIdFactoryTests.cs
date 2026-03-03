using CobolMvpRuntime;
using Xunit;

namespace CobolToCsharpMigration.Tests;

public class StmtIdFactoryTests
{
    [Fact]
    public void Create_WithLineAndColumn_ReturnsFormattedStmtId()
    {
        string stmtId = StmtIdFactory.Create(120, 5);
        Assert.Equal("L000120C005", stmtId);
    }

    [Fact]
    public void Create_WithLineOnly_ReturnsLineOnlyStmtId()
    {
        string stmtId = StmtIdFactory.Create(120);
        Assert.Equal("L000120", stmtId);
    }

    [Fact]
    public void Create_IsStableForSameInput()
    {
        string a = StmtIdFactory.Create(45, 12);
        string b = StmtIdFactory.Create(45, 12);
        Assert.Equal(a, b);
    }
}
