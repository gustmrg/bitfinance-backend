interface ProfileProps {
  user?: any;
  isAuthenticated?: boolean;
  isLoading?: boolean;
}

const Profile = ({
  user,
  isAuthenticated,
  isLoading = false,
}: ProfileProps) => {
  if (isLoading) {
    return <div>Loading ...</div>;
  }

  return (
    isAuthenticated && (
      <div>
        <img src={user?.picture} alt={user?.name} />
        <h2>{user?.name}</h2>
        <p>{user?.email}</p>
      </div>
    )
  );
};

export default Profile;
