const BASE_URL = 'http://localhost:5294/api';

const crudAPI = (basePath: string) => ({
  create: () => `${basePath}`,
  getAll: () => `${basePath}`,
  edit: (id: string | number) => `${basePath}/${id}`,
  delete: (id: string | number) => `${basePath}/${id}`,
  getById: (id: string | number) => `${basePath}/${id}`,
});

const userAPI = crudAPI(`${BASE_URL}/User`);
const authAPI = {
  login: () => `${BASE_URL}/auth/Login`,
  logout: () => `${BASE_URL}/auth/Logout`,
  refreshToken: () => `${BASE_URL}/auth/RefreshToken`,
};

export { userAPI, authAPI };