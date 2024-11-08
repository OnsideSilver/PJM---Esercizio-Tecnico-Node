namespace Node_ApiService_Test.Services
{

        // Abstract class for the CRUD service
        public abstract class CrudServiceBase<TEntity, TDto>
        {
            protected readonly List<TEntity> _entities = new();

            public abstract TDto Create(TDto dto);
            public abstract TDto ReadId(Guid id);
            public abstract TDto ReadName(String name);

            public abstract TDto Update(Guid id, TDto dto);
            public abstract bool DeleteId(Guid id);
            public abstract bool DeleteName(String name);

            protected abstract TEntity MapToEntity(TDto dto);
            protected abstract TDto MapToDto(TEntity entity);
        }
}

