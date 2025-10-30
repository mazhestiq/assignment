const API_BASE_URL = 'https://localhost:7083/api';

export interface Customer {
  id: string;
  name: string;
  email: string | null;
  phone: string | null;
  is_loyal: boolean;
  created_at: string;
  updated_at: string;
}

export interface Hotel {
  id: string;
  name: string;
  location: string | null;
  created_at: string;
}

export interface Visit {
  id: string;
  customer_id: string;
  hotel_id: string;
  visit_date: string;
  customer: {
    id: string;
    name: string;
    is_loyal: boolean;
  } | null;
  hotel: {
    id: string;
    name: string;
  } | null;
}

class ApiClient {
  private async request<T>(endpoint: string, options?: RequestInit): Promise<T> {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      ...options,
      headers: {
        'Content-Type': 'application/json',
        ...options?.headers,
      },
    });

    if (!response.ok) {
      if (response.status === 400) {
        try {
          const errorData = await response.json();
          if (errorData.errors) {
            // Extract all error messages from the errors object
            const errorMessages: string[] = [];
            Object.values(errorData.errors).forEach((messages: any) => {
              if (Array.isArray(messages)) {
                errorMessages.push(...messages);
              }
            });
            throw new Error(errorMessages.join(', '));
          }
        } catch (e) {
          if (e instanceof Error && e.message !== `API error: ${response.statusText}`) {
            throw e;
          }
        }
      }
      throw new Error(`API error: ${response.statusText}`);
    }

    return response.json();
  }

  // Hotels
  async getHotels(): Promise<Hotel[]> {
    return this.request<Hotel[]>('/hotels');
  }

  // Customers
  async getCustomer(id: string): Promise<Customer> {
    return this.request<Customer>(`/customers/${id}`);
  }

  async createCustomer(data: Omit<Customer, 'id' | 'created_at' | 'updated_at'>): Promise<Customer> {
    return this.request<Customer>('/customers', {
      method: 'POST',
      body: JSON.stringify(data),
    });
  }

  async updateCustomer(id: string, data: Partial<Customer>): Promise<Customer> {
    return this.request<Customer>(`/customers/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  }

  // Visits
  async getVisits(params?: {
    hotelIds?: string[];
    monthYear?: Date;
    onlyLoyal?: boolean;
  }): Promise<Visit[]> {
    const searchParams = new URLSearchParams();
    
    if (params?.hotelIds && params.hotelIds.length > 0) {
      searchParams.append('hotel_ids', params.hotelIds.join(','));
    }
    
    if (params?.monthYear) {
      const year = params.monthYear.getFullYear();
      const month = params.monthYear.getMonth();
      const startDate = new Date(year, month, 1).toISOString();
      const endDate = new Date(year, month + 1, 0).toISOString();
      searchParams.append('start_date', startDate);
      searchParams.append('end_date', endDate);
    }
    
    if (params?.onlyLoyal) {
      searchParams.append('only_loyal', 'true');
    }

    const queryString = searchParams.toString();
    const endpoint = queryString ? `/visits?${queryString}` : '/visits';
    
    return this.request<Visit[]>(endpoint);
  }

  async getCustomerVisits(customerId: string): Promise<Visit[]> {
    return this.request<Visit[]>(`/visits/customer/${customerId}`);
  }

  async createVisit(data: { customer_id: string; hotel_id: string; visit_date: string }): Promise<Visit> {
    return this.request<Visit>('/visits', {
      method: 'POST',
      body: JSON.stringify(data),
    });
  }
}

export const apiClient = new ApiClient();
