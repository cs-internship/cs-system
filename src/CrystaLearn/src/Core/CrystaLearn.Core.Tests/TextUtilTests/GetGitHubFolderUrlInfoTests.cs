using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystaLearn.Core.Tests.TextUtilTests;
public class GetGitHubFolderUrlInfoTests
{
    [Fact]
    public void GetGitHubFolderUrlInfo_WhenUrlIsCorrect_ReturnsCorrectInfo()
    {
        // Arrange
        var url = "https://github.com/cs-internship/cs-internship-spec/blob/master/processes/documents";

        // Act
        var info = TextUtil.GetGitHubFolderUrlInfo(url);

        // Assert
        Assert.Equal("cs-internship", info.Owner);
        Assert.Equal("cs-internship-spec", info.RepoName);
        Assert.Equal("master", info.Branch);
        Assert.Equal("processes", info.ParentPath);
        Assert.Equal("processes/documents", info.Path);

    }
}
