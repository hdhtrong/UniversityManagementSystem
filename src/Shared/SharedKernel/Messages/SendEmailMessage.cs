namespace SharedKernel.Messages;

public record SendEmailMessage(string To, string Subject, string Body);