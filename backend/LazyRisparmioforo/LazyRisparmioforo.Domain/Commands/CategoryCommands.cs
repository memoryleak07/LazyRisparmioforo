namespace LazyRisparmioforo.Domain.Commands;

public record CategoryPredictCommand(
    string Input);

public record CategoryPredictResult(
    int Id, 
    string Name, 
    double Confidence);