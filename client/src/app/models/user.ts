import { Basket } from "./baskets";

export interface User {
  email: string;
  token: string;
  basket: Basket;
}
