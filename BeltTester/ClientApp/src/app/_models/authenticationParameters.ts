export interface AuthenticationRequestParameters {
  username: string,
  password: string
}

export interface AuthenticationResponseParameters {
  token: string,
  expiration: Date
}
