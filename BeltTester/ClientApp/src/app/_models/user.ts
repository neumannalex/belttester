export interface User {
  firstname: string,
  lastname: string,
  username: string,
  email: string,
  token: string,
  expiration: Date,
  roles: string[]
}
