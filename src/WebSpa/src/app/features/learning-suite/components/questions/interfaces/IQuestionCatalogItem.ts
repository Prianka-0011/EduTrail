export interface IQuestionCatalogItem {
  slug: string;
  title: string;
  topic: string;
  tags: string[];
  week?: number | null;
  day?: number | null;
}
