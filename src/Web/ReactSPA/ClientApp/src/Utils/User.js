import UserStore from '../Stores/UserStore';

function getUserName(userId) {
    let user = UserStore.get(userId);
    if(!user) return null;

    return user.Name;
}

export {
    getUserName
};