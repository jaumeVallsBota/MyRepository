using Swashbuckle.AspNetCore.Annotations;

public class User
{
    [SwaggerSchema("Identificador único del usuario")]
    public int Id { get; set; }

    [SwaggerSchema("Nombre completo del usuario", Nullable = false)]
    public string Name { get; set; }
}