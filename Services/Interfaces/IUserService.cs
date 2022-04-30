using System.Security;
using ModelsView;

namespace ProgramSystem.Bll.Services.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Получить список всех пользователей
    /// </summary>
    /// <returns></returns>
    IEnumerable<UserView> GetAllUsers();

    /// <summary>
    /// Удалить несколько пользователей (можно и одного) по id
    /// </summary>
    /// <param name="ids"></param>
    /// <returns>Возвращает данные удаленных пользователей</returns>
    Task<IEnumerable<UserView>> RemoveRangeAsync(int[] ids);

    /// <summary>
    /// Изменить данные пользователя (Id обязательно)
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task UpdateUserAsync(UserView item);

    /// <summary>
    /// Добавить пользователя (Id не обязательно)
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task AddUserAsync(UserView item);

    /// <summary>
    /// Получить данные аккаунта (id, роль, логин, пароль) по логину, паролю 
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    UserView? GetAccountByLoginPassword(string login, string password);
}