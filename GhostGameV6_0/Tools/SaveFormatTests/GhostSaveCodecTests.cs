using System;
using System.Text;
using Xunit;

public class GhostSaveCodecTests
{
    [Fact]
    public void RoundTrip_PreservesMissionPositionAndItems()
    {
        var original = new GhostSaveData
        {
            Mission = 4,
            X = -12.5f,
            Y = 1.25f,
            Z = 80f,
            Keys = new[] { "Key1 1", "Gate \"west\"" },
            Notes = new[] { "Note 3", "path\\stone" }
        };

        Assert.True(GhostSaveCodec.TryEncode(original, out byte[] bytes));
        Assert.True(GhostSaveCodec.TryDecode(bytes, out GhostSaveData loaded));
        Assert.Equal(original.Mission, loaded.Mission);
        Assert.Equal(original.X, loaded.X);
        Assert.Equal(original.Y, loaded.Y);
        Assert.Equal(original.Z, loaded.Z);
        Assert.Equal(original.Keys, loaded.Keys);
        Assert.Equal(original.Notes, loaded.Notes);
    }

    [Fact]
    public void Decode_RejectsBinaryFormatterHeader()
    {
        byte[] binaryFormatterHeader = { 0x00, 0x01, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x01, 0x00, 0x00, 0x00 };
        Assert.False(GhostSaveCodec.TryDecode(binaryFormatterHeader, out _));
    }

    [Fact]
    public void Decode_RejectsJsonWithoutMagicHeader()
    {
        byte[] json = Encoding.UTF8.GetBytes("{\"v\":1,\"mission\":0,\"x\":0,\"y\":0,\"z\":0,\"keys\":[],\"notes\":[]}");
        Assert.False(GhostSaveCodec.TryDecode(json, out _));
    }

    [Fact]
    public void Decode_RejectsOversizedPayload()
    {
        byte[] oversized = new byte[GhostSaveCodec.MaxBytes + 1];
        Assert.False(GhostSaveCodec.TryDecode(oversized, out _));
    }

    [Theory]
    [InlineData("{\"v\":1,\"mission\":99,\"x\":0,\"y\":0,\"z\":0,\"keys\":[],\"notes\":[]}")]
    [InlineData("{\"v\":2,\"mission\":0,\"x\":0,\"y\":0,\"z\":0,\"keys\":[],\"notes\":[]}")]
    [InlineData("{\"v\":1,\"mission\":0,\"x\":NaN,\"y\":0,\"z\":0,\"keys\":[],\"notes\":[]}")]
    [InlineData("{\"v\":1,\"mission\":0,\"x\":1e20,\"y\":0,\"z\":0,\"keys\":[],\"notes\":[]}")]
    [InlineData("{\"v\":1,\"mission\":0,\"x\":0,\"y\":0,\"z\":0,\"keys\":[\"<b>x</b>\"],\"notes\":[]}")]
    [InlineData("{\"v\":1,\"mission\":0,\"x\":0,\"y\":0,\"z\":0,\"keys\":[],\"notes\":[],\"extra\":1}")]
    [InlineData("{\"v\":1,\"mission\":0,\"x\":0,\"y\":0,\"z\":0,\"keys\":[],\"notes\":[]}TRAILING")]
    public void Decode_RejectsInvalidDocuments(string json)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(GhostSaveCodec.Magic + "\n" + json);
        Assert.False(GhostSaveCodec.TryDecode(bytes, out _));
    }

    [Fact]
    public void Encode_RejectsUnsafeItemText()
    {
        var data = new GhostSaveData
        {
            Mission = 0,
            X = 0f,
            Y = 1f,
            Z = 0f,
            Keys = new[] { "ok" },
            Notes = new[] { new string('a', GhostSaveCodec.MaxStringLength + 1) }
        };

        Assert.False(GhostSaveCodec.TryEncode(data, out _));
    }

    [Fact]
    public void Decode_RejectsTooManyItems()
    {
        var items = new StringBuilder();
        items.Append('[');
        for (int i = 0; i < GhostSaveCodec.MaxItems + 1; i++)
        {
            if (i > 0)
                items.Append(',');
            items.Append("\"k\"");
        }
        items.Append(']');

        string json = "{\"v\":1,\"mission\":0,\"x\":0,\"y\":0,\"z\":0,\"keys\":" + items + ",\"notes\":[]}";
        byte[] bytes = Encoding.UTF8.GetBytes(GhostSaveCodec.Magic + "\n" + json);
        Assert.False(GhostSaveCodec.TryDecode(bytes, out _));
    }
}
