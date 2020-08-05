using Shop.Models;

namespace Shop.Tests.Bulders
{
    public class Userbuilder: Builder<User>
    {
        public Userbuilder WithId(string Id)
        {
            _object.Id = Id;
            return this;
        }

        public Userbuilder WithUserName(string userName)
        {
            _object.UserName = userName;
            return this;
        }

        public Userbuilder WithEmail(string email)
        {
            _object.Email= email;
            return this;
        }
    }
}
