variable "resource_group_name" {
  type        = string
  description = "Resource group for Terraform state."
  default     = "reactfullstackdemo-tfstate-rg"
}

variable "location" {
  type        = string
  description = "Azure region."
  default     = "eastus"
}

variable "storage_account_name" {
  type        = string
  description = "Globally unique storage account name."
  default     = "rfsdtfstate"
}

variable "container_name" {
  type        = string
  description = "Blob container name."
  default     = "tfstate"
}
