-- Drop the trigger first
DROP TRIGGER IF EXISTS update_customers_timestamp ON public.customers;

-- Drop the function
DROP FUNCTION IF EXISTS public.update_customer_timestamp();

-- Recreate the function with proper search_path
CREATE OR REPLACE FUNCTION public.update_customer_timestamp()
RETURNS TRIGGER 
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public
AS $$
BEGIN
  NEW.updated_at = now();
  RETURN NEW;
END;
$$;

-- Recreate the trigger
CREATE TRIGGER update_customers_timestamp
BEFORE UPDATE ON public.customers
FOR EACH ROW
EXECUTE FUNCTION public.update_customer_timestamp();