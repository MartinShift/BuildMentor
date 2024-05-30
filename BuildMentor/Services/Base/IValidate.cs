namespace BuildMentor.Services.Base
{
    public interface IValidate<T>
    {
        string Validate(T value);
    }
}
