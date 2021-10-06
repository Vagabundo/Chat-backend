# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 2.65"
    }
  }

  required_version = ">= 0.14.9"
}

terraform {
  backend "azurerm" {
    resource_group_name  = "tf_rg_blobstore"
    storage_account_name = "tfstorageaccvaga"
    container_name       = "tfstate"
    key                  = "terraform.tfstate"
  }
}

# terraform {
#   backend "remote" {
#     organization = "Vagacorp"

#     workspaces {
#       name = "web-chat"
#     }
#   }
# }

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location

  tags = {
    Environment = "Web chat"
    Team        = "VagaDevOps"
  }
}

resource "azurerm_app_service_plan" "freeplan" {
  name                = "${var.resource_group_name}-plan"
  location            = var.location
  resource_group_name = var.resource_group_name

  sku {
    tier = "Free"
    size = "F1"
  }
}

resource "azurerm_container_group" "webchat_containergroup" {
  name                = "${var.resource_group_name}-containergroup"
  location            = var.location
  resource_group_name = var.resource_group_name

  ip_address_type = "public"
  dns_name_label  = "vagabundo-${var.project_name}"
  os_type         = "Linux"

  container {
    name   = var.project_name
    image  = "vagabundocker/${var.project_name}:${var.imagebuild}"
    cpu    = "1"
    memory = "1"

    ports {
      port     = 80
      protocol = "TCP"
    }
  }

  tags = {
    Environment = "Web chat"
  }
}