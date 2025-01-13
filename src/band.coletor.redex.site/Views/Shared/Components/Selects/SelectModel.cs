using System.Collections.Generic;

public class ComponentColetorSelectModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int? Value { get; set; }
    public string Descricao { get; set; }
    public bool Disabled { get; set; }
    public List<SelectOption> Options { get; set; } = new List<SelectOption>();
}

public class SelectOption
{
    public int Value { get; set; }
    public string Text { get; set; }
}
