variable "name_prefix" {
  type        = string
  description = "Prefix used for Azure resources."
  default     = "react-fullstack-demo"
}

variable "location" {
  type        = string
  description = "Azure region."
  default     = "canadacentral"
}

variable "api_image_name" {
  type        = string
  description = "Container image name for the API."
  default     = "react-fullstack-demo-api"
}

variable "api_image_tag" {
  type        = string
  description = "Container image tag for the API."
  default     = "v1"
}

variable "web_image_name" {
  type        = string
  description = "Container image name for the web app."
  default     = "react-fullstack-demo-web"
}

variable "web_image_tag" {
  type        = string
  description = "Container image tag for the web app."
  default     = "v1"
}

variable "angular_image_name" {
  type        = string
  description = "Container image name for the Angular web app."
  default     = "react-fullstack-demo-angular"
}

variable "angular_image_tag" {
  type        = string
  description = "Container image tag for the Angular web app."
  default     = "v1"
}

variable "mongo_database" {
  type        = string
  description = "MongoDB database name."
  default     = "react_fullstack_demo"
}
