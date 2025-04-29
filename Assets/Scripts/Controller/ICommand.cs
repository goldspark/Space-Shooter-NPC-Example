using UnityEngine;

public interface ICommand<T, K>
{
    void Exec(T entity, K data);
}
