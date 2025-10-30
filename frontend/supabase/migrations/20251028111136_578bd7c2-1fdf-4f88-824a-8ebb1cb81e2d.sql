-- Create hotels table
CREATE TABLE public.hotels (
  id UUID NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
  name TEXT NOT NULL,
  location TEXT,
  created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now()
);

-- Create customers table
CREATE TABLE public.customers (
  id UUID NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
  name TEXT NOT NULL,
  email TEXT,
  phone TEXT,
  is_loyal BOOLEAN NOT NULL DEFAULT false,
  created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
  updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now()
);

-- Create visits table
CREATE TABLE public.visits (
  id UUID NOT NULL DEFAULT gen_random_uuid() PRIMARY KEY,
  customer_id UUID NOT NULL REFERENCES public.customers(id) ON DELETE CASCADE,
  hotel_id UUID NOT NULL REFERENCES public.hotels(id) ON DELETE CASCADE,
  visit_date DATE NOT NULL,
  created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now()
);

-- Enable Row Level Security
ALTER TABLE public.hotels ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.customers ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.visits ENABLE ROW LEVEL SECURITY;

-- Create policies for public access (since this is an admin tool)
CREATE POLICY "Public read access for hotels" 
ON public.hotels FOR SELECT 
USING (true);

CREATE POLICY "Public insert access for hotels" 
ON public.hotels FOR INSERT 
WITH CHECK (true);

CREATE POLICY "Public read access for customers" 
ON public.customers FOR SELECT 
USING (true);

CREATE POLICY "Public insert access for customers" 
ON public.customers FOR INSERT 
WITH CHECK (true);

CREATE POLICY "Public update access for customers" 
ON public.customers FOR UPDATE 
USING (true);

CREATE POLICY "Public read access for visits" 
ON public.visits FOR SELECT 
USING (true);

CREATE POLICY "Public insert access for visits" 
ON public.visits FOR INSERT 
WITH CHECK (true);

-- Create indexes for better query performance
CREATE INDEX idx_visits_customer_id ON public.visits(customer_id);
CREATE INDEX idx_visits_hotel_id ON public.visits(hotel_id);
CREATE INDEX idx_visits_date ON public.visits(visit_date);
CREATE INDEX idx_customers_loyal ON public.customers(is_loyal);

-- Create function to update customer updated_at
CREATE OR REPLACE FUNCTION public.update_customer_timestamp()
RETURNS TRIGGER AS $$
BEGIN
  NEW.updated_at = now();
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Create trigger for automatic timestamp updates
CREATE TRIGGER update_customers_timestamp
BEFORE UPDATE ON public.customers
FOR EACH ROW
EXECUTE FUNCTION public.update_customer_timestamp();

-- Insert some sample hotels
INSERT INTO public.hotels (name, location) VALUES
  ('Grand Plaza Hotel', 'New York, NY'),
  ('Seaside Resort & Spa', 'Miami, FL'),
  ('Mountain View Lodge', 'Denver, CO'),
  ('Urban Boutique Hotel', 'San Francisco, CA');