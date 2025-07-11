import type { OwnerDto } from "./OwnerDto";

export interface PropertyDto {
    id?: string;
    idOwner?: string;
    name?: string;
    address?: string;
    price?: number;
    imageUrl?: string;
    owner?: OwnerDto;
}
